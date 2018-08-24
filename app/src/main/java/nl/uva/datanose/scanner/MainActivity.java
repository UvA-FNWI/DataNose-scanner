package nl.uva.datanose.scanner;

import android.Manifest;
import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.SharedPreferences;
import android.content.pm.PackageManager;
import android.os.AsyncTask;
import android.os.Bundle;

import android.support.annotation.NonNull;
import android.support.design.widget.BottomNavigationView;
import android.support.v4.app.ActivityCompat;
import android.support.v4.content.ContextCompat;
import android.support.v7.app.AppCompatActivity;
import android.text.InputType;

import android.view.LayoutInflater;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;

import com.google.zxing.Result;


import me.dm7.barcodescanner.zxing.ZXingScannerView;



public class MainActivity extends AppCompatActivity  implements ZXingScannerView.ResultHandler, DialogInterface.OnClickListener, DialogInterface.OnDismissListener, View.OnClickListener {


    private class CheckKeyTask extends AsyncTask<Void, Integer, DataNoseKeyResponse> implements DialogInterface.OnClickListener {

        EditText input;

        @Override
        protected DataNoseKeyResponse doInBackground(Void... v) {
            return connector.tryKey();
        }

        protected void onPostExecute(DataNoseKeyResponse response) {

            if(!response.status.equals("valid-key"))
            {
                mTextMessage.setText("Voer eerst een geldige sleutel in.");
                AlertDialog.Builder builder = new AlertDialog.Builder(MainActivity.this);
                builder.setTitle("Voer een sleutel in:");

                input = new EditText(MainActivity.this);
                input.setInputType(InputType.TYPE_CLASS_TEXT | InputType.TYPE_TEXT_VARIATION_PASSWORD);
                builder.setView(input);

                builder.setPositiveButton("Opslaan", CheckKeyTask.this);

                builder.show();
            }
            else { // Valid key!

                mTextMessage.setText(response.message);
                navigation.setVisibility(View.VISIBLE);
            }
        }


        @Override
        public void onClick(DialogInterface dialogInterface, int i) {
            String key = input.getText().toString();
            sharedPreferences.edit().putString("key", key).apply();
            connector.setKey(key);
            new CheckKeyTask().execute();
        }
    }

    private class CheckQRCode extends AsyncTask<String, Integer, DataNoseCodeResponse> {

        @Override
        protected DataNoseCodeResponse doInBackground(String... strings) {
            return connector.tryCode(strings[0]);
        }

        protected void onPostExecute(DataNoseCodeResponse response) {

            AlertDialog.Builder builder = new AlertDialog.Builder(MainActivity.this);

            LayoutInflater inflater = MainActivity.this.getLayoutInflater();
            View view = inflater.inflate(R.layout.dialog, null);
            builder.setView(view);

            TextView name = (TextView) view.findViewById(R.id.dialog_name);
            name.setText(response.student);

            TextView programme = (TextView) view.findViewById(R.id.dialog_programme);
            programme.setText(response.programme);

            TextView remarks = (TextView) view.findViewById(R.id.dialog_message);
            remarks.setText(response.remarks);

            if(!response.status.equals("scan-ok")) {
                name.setVisibility(View.GONE);
                programme.setVisibility(View.GONE);
            }

            if(response.remarks.isEmpty())
                remarks.setVisibility(View.GONE);

            builder.setPositiveButton("Volgende", MainActivity.this);
            builder.setOnDismissListener(MainActivity.this);
            builder.show();
        }
    }


    private TextView mTextMessage;
    private ZXingScannerView mScannerView;
    SharedPreferences sharedPreferences;

    DataNoseConnector connector;


    private BottomNavigationView.OnNavigationItemSelectedListener mOnNavigationItemSelectedListener
            = new BottomNavigationView.OnNavigationItemSelectedListener() {

        @Override
        public boolean onNavigationItemSelected(@NonNull MenuItem item) {
            switch (item.getItemId()) {
                case R.id.navigation_home:
                    mScannerView.stopCamera();
                    mScannerView.setVisibility(View.INVISIBLE);
                    return true;
                case R.id.navigation_scan:
                    mScannerView.setVisibility(View.VISIBLE);
                    mScannerView.startCamera();
                    return true;
            }
            return false;
        }
    };

    BottomNavigationView navigation;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        sharedPreferences = this.getPreferences(Context.MODE_PRIVATE);

        if(ContextCompat.checkSelfPermission(this, Manifest.permission.CAMERA) != PackageManager.PERMISSION_GRANTED)
        {
            ActivityCompat.requestPermissions(this, new String[] {Manifest.permission.CAMERA}, 100);

        }

        connector = new DataNoseConnector(sharedPreferences.getString( "key",""));

        setContentView(R.layout.activity_main);

        mScannerView = (ZXingScannerView) findViewById(R.id.scannerview);
        mScannerView.setResultHandler(this);


        mTextMessage = (TextView) findViewById(R.id.message);
        new CheckKeyTask().execute();

        Button button = (Button) findViewById(R.id.testknop);
        button.setVisibility(View.GONE);
//        button.setOnClickListener(this);

        navigation = (BottomNavigationView) findViewById(R.id.navigation);
        navigation.setVisibility(View.INVISIBLE);
        navigation.setOnNavigationItemSelectedListener(mOnNavigationItemSelectedListener);
    }

    public void handleResult(Result result) {
        new CheckQRCode().execute(result.getText());

    }

    @Override
    public void onClick(View view) {

        new CheckQRCode().execute("test");
    }

    @Override
    public void onClick(DialogInterface dialogInterface, int i) {

        mScannerView.resumeCameraPreview(this);
    }

    @Override
    public void onDismiss(DialogInterface dialogInterface) {
        mScannerView.resumeCameraPreview(this);
    }
}
