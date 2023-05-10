# 62597-Backend-F23
Backend for 62597 Course at DTU

## URLs for our server side API, webshop and dashboards
To try our REST API, you can use the following URLs:

* https://dtu-api.herogamers.dev/api - This is the URL for our ASP.NET REST API. You can send HTTP requests to this URL to interact with our API endpoints.

To try our webshop and dashboards, use the following URLs:

* https://dtu.herogamers.dev - This is the URL for our frontend webshop, developed as part of our front end course. You can browse the webshop and place orders using this URL.
* https://dtu-grafana.herogamers.dev - This is the URL for our Grafana dashboard. You can use this dashboard to monitor various metrics related to our system.
* https://dtu-prometheus.herogamers.dev - This is the URL for our Prometheus dashboard. You can use this dashboard to view detailed metrics about our system.

## Running the backend locally
Follow the following steps to start the backend, or just run it locally:

1. Go into the `app` and `monitoring` directories, and copy the environment files, and change the extension from `*.env.example` to `*.env`. Modify the files as needed.
2. Modify `proxy/docker-compose.yml` to point at the correct directory for the SSL certificates, or place your SSL certificates in `/home/project/ssl` on your host machine, and put the private key in `private/key.pem` and the public key in `certs/cert.pem`.
3. If running the backend on another domain, change the Nginx configuration files (`proxy/nginx/conf.d/servers/*.conf`) to listen to another domain (**NOT NEEDED WHEN RUNNING LOCALLY!!!**).
4. Install the [Docker Engine and docker-compose](https://docs.docker.com/get-docker/), if not installed already.
5. Run `sh ./start.sh` to start all the services in the backend.
6. You can now access the backend on `localhost:3000`.
7. Bonus: If you want to access Grafana locally too, then add a new server listener in `proxy/nginx/conf.d/servers/dtu-grafana.conf`, and add the port mapping to `proxy/docker-compose.yml`.
