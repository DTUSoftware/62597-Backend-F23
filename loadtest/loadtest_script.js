import http from "k6/http"
import {sleep} from "k6"
import { SharedArray } from 'k6/data';

//const apiUrl = "http://localhost:5114/api"
const apiUrl = "https://dtu-api.herogamers.dev/api"


const data = new SharedArray('products', function () {
  const f = JSON.parse(open('./products.json'));
  return f;
});

export function generate_uuidv4() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g,
    function(c) {
       var uuid = Math.random() * 16 | 0, v = c == 'x' ? uuid : (uuid & 0x3 | 0x8);
       return uuid.toString(16);
    });
 }

 export function getRandomInt(min, max) {
    min = Math.ceil(min);
    max = Math.floor(max);
    return Math.floor(Math.random() * (max - min + 1) + min);
  }

export let options = {
    vus: 3600,
    duration: "600s",
    thresholds: {
        http_req_duration: ["p(95)<1500"]
    }
};

export default function() {
      var params = {
        headers: {
            "Content-Type": "application/json",
        }
      }

      const order = JSON.stringify({
        "checkMarketing": true,
        "submitComment": generate_uuidv4(),
        "shippingAddress": {
          "firstName": generate_uuidv4(),
          "lastName": generate_uuidv4(),
          "email": generate_uuidv4(),
          "mobileNr": generate_uuidv4(),
          "company": generate_uuidv4(),
          "vatNr": generate_uuidv4(),
          "country": generate_uuidv4(),
          "zipCode": generate_uuidv4(),
          "city": generate_uuidv4(),
          "address1": generate_uuidv4(),
          "address2": generate_uuidv4(),
        },
        "billingAddress": {
          "firstName": generate_uuidv4(),
          "lastName": generate_uuidv4(),
          "email": generate_uuidv4(),
          "mobileNr": generate_uuidv4(),
          "company": generate_uuidv4(),
          "vatNr": generate_uuidv4(),
          "country": generate_uuidv4(),
          "zipCode": generate_uuidv4(),
          "city": generate_uuidv4(),
          "address1": generate_uuidv4(),
          "address2": generate_uuidv4(),
        },
        "orderDetails": [
          {
            "quantity": getRandomInt(1,10),
            "giftWrap": true,
            "recurringOrder": false,
            "productId": data[getRandomInt(0,28)].id,
          }
        ]
      })

      sleep(getRandomInt(0,30));
      http.get(`${apiUrl}/products/all/${getRandomInt(0,1)}`);
      sleep(getRandomInt(5,30));
      http.get(`${apiUrl}/products/${data[getRandomInt(0,28)].id}`);
      sleep(getRandomInt(5,30));
      http.post(`${apiUrl}/orders`, order, params);
      sleep(getRandomInt(5,30));
}