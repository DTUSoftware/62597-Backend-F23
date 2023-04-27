import http from "k6/http"
import {sleep} from "k6"

const apiUrl = "http://localhost:5114/api"

export function generate_uuidv4() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g,
    function(c) {
       var uuid = Math.random() * 16 | 0, v = c == 'x' ? uuid : (uuid & 0x3 | 0x8);
       return uuid.toString(16);
    });
 }

export let options = {
    vus: 100,
    duration: "30s",
    thresholds: {
        http_req_duration: ["p(95)<1500"]
    }
};

export default function() {
    const payload = JSON.stringify({
        "id": generate_uuidv4(),
        "name": generate_uuidv4(),
        "price": 0,
        "currency": generate_uuidv4(),
        "rebateQuantity": 0,
        "rebatePercent": 0,
        "upsellProductId": generate_uuidv4(),
        "imageUrl": generate_uuidv4(),
      })

      var params = {
        headers: {
            "Content-Type": "application/json",
            "Authorization": "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJzdHJpbmdAc2Rmc2RmIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQWRtaW4iLCJleHAiOjE2ODI2Mjc5OTYsImlzcyI6IlNob3BCYWNrZW5kIn0.7dEE7BO_2aspLnxWR2TxvcPqolb_YUZmQiOycz7hqZo"
        }
      }

      http.post(`${apiUrl}/products`, payload, params);
      sleep(1);
}