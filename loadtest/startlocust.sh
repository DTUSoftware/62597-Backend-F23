echo "Starting Master and workers for locust test:"

locust -f locustfile.py --master --expect-workers=8 &
./locustworker.sh &
./locustworker.sh &
./locustworker.sh &
./locustworker.sh &
./locustworker.sh &
./locustworker.sh &
./locustworker.sh &
./locustworker.sh