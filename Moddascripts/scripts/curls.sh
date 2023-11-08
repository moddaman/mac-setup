   curl -i -u "fdirirjv:bTYCtFuLQYRxnHdTcdQE1seqJoIQuWC7" -H "content-type:application/json" \
        -XPUT -d'{"type":"x-consistent-hash","durable":true}' \
      "https://sleepy-teal-cougar.rmq.cloudamqp.com:443/api/exchanges/%2F/test1234"


curl -i -u "easeeadmin:9cKafZBoAf8TTrFSfVAYWgA" -H "content-type:application/json" \
        -XPOST  -d \ '{"routing_key":"1234", "arguments":{"x-arg": "value"}}' \
        "https://rabbitmq.easee.local:15671/api/bindings/%2F/e/Observation.X/q/Observation0.Q"

         curl -i -u "easeeadmin:9cKafZBoAf8TTrFSfVAYWgA" -H "content-type:application/json" \
        -XPUT -d'{"type":"x-consistent-hash","durable":true}' \
      "https://rabbitmq.easee.local:15671/api/exchanges/%2F/2dsadas"


         curl -i -u "easeeadmin:9cKafZBoAf8TTrFSfVAYWgA" "https://rabbitmq.easee.local:15671/api/extensions"
   

    curl -i -u "fdirirjv:bTYCtFuLQYRxnHdTcdQE1seqJoIQuWC7" "https://sleepy-teal-cougar.rmq.cloudamqp.com:443/api/extensions"


             curl -i -u "easeeadmin:9cKafZBoAf8TTrFSfVAYWgA" "https://rabbitmq.easee.local:15671/api/nodes"



curl -u "fdirirjv:bTYCtFuLQYRxnHdTcdQE1seqJoIQuWC7" \
  -d "plugin_name=rabbitmq_top" \
  https://sleepy-teal-cougar.rmq.cloudamqp.com/api/plugins



    curl -i -u "easeeadmin:9cKafZBoAf8TTrFSfVAYWgA" -H "content-type:application/json" \
        -XPOST  -d \ '{"routing_key":"1"}' \
        "rabbitmq.easee.local:15671/api/bindings/%2F/e/Observation.X/q/Observation0.Q"



        /api/bindings/vhost/e/exchange/q/queue/props



curl -i -u "easeeadmin:9cKafZBoAf8TTrFSfVAYWgA" -H "content-type:application/json" \
        -XPOST  -d \ '{"routing_key":"1234", "arguments":{"x-arg": "value"}}' \
        "rabbitmq.easee.local:15671/api/bindings/%2F/e/Historian.X/q/test.Q"