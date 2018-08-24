package nl.uva.datanose.scanner;


import android.util.Log;

import org.json.JSONException;
import org.json.JSONObject;

import java.io.IOException;

import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.Response;

class DataNoseKeyResponse {
    String status;
    String message;

    public DataNoseKeyResponse(Response response) throws IOException, JSONException {

        JSONObject json = new JSONObject(response.body().string());

        status = json.getString("status");
        message = json.getString("message");
    }
}

class DataNoseCodeResponse {
    String status;
    String id;
    String student;
    String programme;
    String remarks;

    public DataNoseCodeResponse(Response response) throws IOException, JSONException {

        String body = response.body().string();
        Log.e("Got:", body);

        JSONObject json = new JSONObject(body);

        status = json.getString("status");
        id = json.getString("id");
        student = json.getString("student");
        programme = json.getString("programme");
        remarks = json.getString("remarks");
    }
}

public class DataNoseConnector {

    public static final String API_URL = "https://api-acc.datanose.nl/ScannerApp?key=";
    private OkHttpClient client;
    public String key;

    public DataNoseConnector(String key){
        client = new OkHttpClient();
        this.key = key;
    }

    public void setKey(String key) {
        this.key = key;
    }

    public DataNoseKeyResponse tryKey()
    {
        Request request = new Request.Builder().url(API_URL + key).build();

        try {
            return new DataNoseKeyResponse(client.newCall(request).execute());

        } catch (IOException e) {
            e.printStackTrace();

        } catch (JSONException e) {
            e.printStackTrace();
        }

        return null;
    }

    public DataNoseCodeResponse tryCode(String code) {
        Request request = new Request.Builder().url(API_URL + key + "&code=" + code).build();

        try {
            return new DataNoseCodeResponse(client.newCall(request).execute());

        } catch (IOException e) {
            e.printStackTrace();

        } catch (JSONException e) {
            e.printStackTrace();
        }

        return null;
    }

}
