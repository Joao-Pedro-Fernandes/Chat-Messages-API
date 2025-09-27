#! /bin/bash
echo "Fazendo deploy do backend..."
PATH_PROJECT="/home/joaom/Chat-Messages-API/Chat-Messages-API"
cd "$PATH_PROJECT"
git checkout main
git pull
docker build -t app-api-chat-messages-image "$PATH_PROJECT"
sudo ctr images pull app-api-chat-messages-image:latest
kubectl delete pod -l app=app-api-chat-messages
