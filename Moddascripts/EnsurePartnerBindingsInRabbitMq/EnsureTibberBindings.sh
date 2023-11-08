#!/bin/bash

# How to use:
# chmod +x ./EnsureTibberBindings.sh
# ./EnsureTibberBindings.sh [username] [password] [Charger serial number]

echo "Ensuring tibber bindings production "
Port=15671
Host="https://glad-gray-pug.rmq.cloudamqp.com"
HostAndPort="${Host}:${Port}"
UsernameAndPassword="$1:$2"
device=$3
IFS='|'

TemplateSubscribeExchange="TS.EASEECHG.X"
TemplatePublishExchange="TP.EASEECHG.X"
tibberWhitelist="MCS_Easee@@@Partner_6.W"
routingKey="$device.#"

echo "Ensuring tibber bindings for host {$Host} on port {$Port}"

echo "Creating binding from  {$TemplateSubscribeExchange} to {$tibberWhitelist} with routing key: {$routingKey}.#"
   curl -i -u "$UsernameAndPassword" -H "content-type:application/json" \
        -XPOST  -d \ '{"routing_key":"'"$routingKey"'"}' \
        "{$HostAndPort}/api/bindings/%2F/e/{$TemplateSubscribeExchange}/e/{$tibberWhitelist}"
echo "success"

echo "Creating binding from  {$tibberWhitelist} to {$TemplatePublishExchange} with routing key: {$routingKey}.#"
    curl -i -u "$UsernameAndPassword" -H "content-type:application/json" \
        -XPOST  -d \ '{"routing_key":"'"$routingKey"'"}' \
        "{$HostAndPort}/api/bindings/%2F/e/{$tibberWhitelist}/e/{$TemplatePublishExchange}"
echo "Finished"
