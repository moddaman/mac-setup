#!/bin/bash

Host="$1"
User="$2"
Password="$3"
Port=15672
HostAndPort="${Host}:${Port}"
UsernameAndPassword="$2:$3"
IFS='|'


#https://rawcdn.githack.com/rabbitmq/rabbitmq-server/v3.8.12/deps/rabbitmq_management/priv/www/api/index.html

echo "Running: Configuring rabbitMQ as we want on $HostAndPort"
# "TS.EASEECHG.X" "TP.EASEECHG.X"
declare -a topicExchanges=("mqtt.topic" "Historian.X")
declare -a consistentHashExchanges=("ChopperCHE.X")
declare -a queues=("Chopper1A.Q", "Observation.Q", "Command.Q", "Pulse.Q")

# FromExchange|ToExchange|BindingKey 
# declare -a exchangeToExchangeBindings=("TS.EASEECHG.X|ChopperCHE.X|*.OP.#")

# FromExchange|ToQueue|BindingKey
declare -a exchangeToQueueBindings=("ChopperCHE.X|Chopper1A.Q|3", "Historian.X|Chopper1A.Q|*.OP.#", "Historian.X|Observation.Q|*.O.*", "Historian.X|Command.Q|*.C.#", "Historian.X|Command.Q|*.CR.#", "Historian.X|Pulse.Q|*.P", "Historian.X|Pulse.Q|*.P.*")


echo "Creating Topic Exchanges"
for i in "${topicExchanges[@]}"
do
   curl -i -u "$UsernameAndPassword" -H "content-type:application/json" \
     -XPUT -d'{"type":"topic","durable":true}' \
     "{$HostAndPort}/api/exchanges/%2F/{$i}"
done

echo "Creating consistent-hash-exchange Exchanges"
for i in "${consistentHashExchanges[@]}"
do
   curl -i -u "$UsernameAndPassword" -H "content-type:application/json" \
        -XPUT -d'{"type":"x_consistent_hash","durable":true}' \
      "{$HostAndPort}/api/exchanges/%2F/{$i}"
done


echo "create queues"

for i in "${queues[@]}"
do
   curl -i -u "$UsernameAndPassword" -H "content-type:application/json" \
        -XPUT -d'{"auto_delete":"false","durable":true}' \
        "{$HostAndPort}/api/queues/%2F/{$i}"
done


echo "create exchange to exchange bindings"

for i in "${exchangeToExchangeBindings[@]}"
do
    # arr=$(echo $i | tr "|" "\n")
    echo ${i}
    read -a elements <<< "$i"
    curl -i -u "$UsernameAndPassword" -H "content-type:application/json" \
        -XPOST  -d \ '{"routing_key":"'${elements[2]}'"}' \
        "{$HostAndPort}/api/bindings/%2F/e/${elements[0]}/e/${elements[1]}"
done


echo "create exchange to queue bindings"

for i in "${exchangeToQueueBindings[@]}"
do
    # arr=$(echo $i | tr "|" "\n")
    echo ${i}
    read -a elements <<< "$i"
    curl -i -u "$UsernameAndPassword" -H "content-type:application/json" \
        -XPOST  -d \ '{"routing_key":"'${elements[2]}'"}' \
        "{$HostAndPort}/api/bindings/%2F/e/${elements[0]}/q/${elements[1]}"
done







