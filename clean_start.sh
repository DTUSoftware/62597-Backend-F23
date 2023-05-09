docker-compose -f ./app/docker-compose.yml up -d --build --force-recreate
docker-compose -f ./monitoring/docker-compose.yml up -d --build --force-recreate
docker-compose -f ./proxy/docker-compose.yml up -d --build --force-recreate
