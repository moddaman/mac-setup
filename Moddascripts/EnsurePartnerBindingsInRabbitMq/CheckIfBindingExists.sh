#!/bin/bash

# How to use:
# chmod +x ./CheckIfBindingExists.sh
# ./CheckIfBindingExists.sh [username] [password] [ serial number]


Port=15671
Host="https://glad-gray-pug.rmq.cloudamqp.com"
HostAndPort="${Host}:${Port}"
UsernameAndPassword="$1:$2"
device=$3
IFS='|'
TemplateSubscribeExchange="TS.EASEECHG.X"
TemplatePublishExchange="TP.EASEECHG.X"
#partner="10896" #Tibber=6
whitelist="MCS_Easee@@@Partner_10896.W"

echo "Checking tibber bindings for host {$Host} on port {$Port}. {$whitelist}"



curl -i -u "$UsernameAndPassword" "$HostAndPort/api/bindings/%2F/e/$TemplateSubscribeExchange/e/$whitelist/$device.%23" >> ${device}.txt 
curl -i -u "$UsernameAndPassword" "$HostAndPort/api/bindings/%2F/e/$whitelist/e/$TemplatePublishExchange/$device.%23" >> ${device}.txt