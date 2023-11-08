## Configure RabbitMQ

- Open conifgureRabbtiMQ.sh and add Hostname and password
- chmod 700 conifgureRabbtiMQ.sh
- ./conifgureRabbtiMQ.sh [Url] [username] [Password]

e.g for local stack:

./conifgureRabbtiMQ.sh rabbitmq.easee.local easeeadmin 9cKafZBoAf8TTrFSfVAYWgA

- Create cloudAMQP node
- Send mail and ask them to open port:
- Create exchange Historian.X
- Create Exchange mqtt.topic
- https://github.com/easee/easee-localstack/blob/master/rabbitmq/broker_definitions.json
- https://github.com/easee/easee-localstack/blob/master/rabbitmq/rabbitmq.conf 15671 / 15672
- Chopper worker
