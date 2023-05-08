from locust import FastHttpUser, task,between
from pathlib import Path
import time, random, uuid, json

# GUI: 
# http://localhost:8089
# API: 
# https://dtu-api.herogamers.dev/api
# Installation Guide:
# https://docs.locust.io/en/stable/installation.html

# Get local 
filepath = Path(__file__).parent / "products.json"
with open(filepath) as fp:
    data = json.load(fp)

class LocustUser(FastHttpUser):
    wait_time = between(5, 15)
    @task
    def get_products(self):
        self.client.get("/products/all/"+str(random.randint(0,1)))

    @task
    def post_order(self):
        self.client.post("/orders",
                        json={
        "checkMarketing": True,
        "submitComment": str(uuid.uuid4()),
        "shippingAddress": {
          "firstName": str(uuid.uuid4()),
          "lastName": str(uuid.uuid4()),
          "email": str(uuid.uuid4()),
          "mobileNr": str(uuid.uuid4()),
          "company": str(uuid.uuid4()),
          "vatNr": str(uuid.uuid4()),
          "country": str(uuid.uuid4()),
          "zipCode": str(uuid.uuid4()),
          "city": str(uuid.uuid4()),
          "address1": str(uuid.uuid4()),
          "address2": str(uuid.uuid4())
        },
        "billingAddress": {
          "firstName": str(uuid.uuid4()),
          "lastName": str(uuid.uuid4()),
          "email": str(uuid.uuid4()),
          "mobileNr": str(uuid.uuid4()),
          "company": str(uuid.uuid4()),
          "vatNr": str(uuid.uuid4()),
          "country": str(uuid.uuid4()),
          "zipCode": str(uuid.uuid4()),
          "city": str(uuid.uuid4()),
          "address1": str(uuid.uuid4()),
          "address2": str(uuid.uuid4())
        },
        "orderDetails": [
          {
            "quantity": random.randint(1,10),
            "giftWrap": True,
            "recurringOrder": False,
            "productId": str(data[random.randint(0,28)]["id"]),
          }
        ]
      })
        
# Read local json file:
# https://stackoverflow.com/questions/62818625/read-local-json-file-with-python
# https://stackoverflow.com/questions/70175221/cant-open-file-in-the-same-directory