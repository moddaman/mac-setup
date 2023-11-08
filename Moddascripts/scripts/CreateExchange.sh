   HostAndPort="https://glad-gray-pug.rmq.cloudamqp.com:15671"
   UsernameAndPassword="boxmthis:Ky7Ze0G8YdXOVpwLpv-lLfCeBgn6-dIA"
   
   curl -i -u "$UsernameAndPassword" -H "content-type:application/json" \
     -XPUT -d'{"type":"topic","durable":true}' \
     "{$HostAndPort}/api/exchanges/%2F/TP.EASEE.SX"

   curl -i -u "$UsernameAndPassword" -H "content-type:application/json" \
      -XPOST  -d \ '{"routing_key":"#"}' \
      "{$HostAndPort}/api/bindings/%2F/e/TP.EASEE.SX/e/TP.EASEECHG.X"


   curl -i -u "$UsernameAndPassword" -H "content-type:application/json" \
      -XPOST  -d \ '{"routing_key":"#"}' \
      "{$HostAndPort}/api/bindings/%2F/e/TP.EASEE.SX/e/TP.EASEEEQZ.X"