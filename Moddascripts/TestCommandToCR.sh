echo "Start smoke test"
#WORKS
# if curl --request POST \
#   --url https://j718xkzv7l.execute-api.eu-west-1.amazonaws.com/smoke-test \
#   --header 'Authorization: 57761876-4e50-478d-9bc8-fb9633004802' \
#   --header 'Content-Type: application/json' \
#   --data '{
# 	"serialNumber": "ECKQZ9YL",
# 	"smokeTestKey": "current-device-state-basic"
# }'; then
#     echo "curl command was successful"
# else
#     echo "curl command failed"
# fi
response=$(curl -s -w "%{http_code}" -X POST \
  --url "https://j718xkzv7l.execute-api.eu-west-1.amazonaws.com/smoke-test" \
  --header "Authorization: 57761876-4e50-478d-9bc8-fb9633004802" \
  --header "Content-Type: application/json" \
  --data '{"serialNumber": "ECKQZ9YL","smokeTestKey": "current-device-state-basic"}')



send_command_status_code=${response: -3}
echo "send_command_status_code 2: $send_command_status_code"
response_body=${response:0:${#response}-3}
echo "Response 2: $response_body"
if [ "$send_command_status_code" -eq 200 ]; then
    echo "Status code is 200 - Request successful."
else
    echo "Status code is not 200 - Request failed."
    exit 1
fi


echo "Status code: $status_code"
echo "Response body: $response_body"

echo "done"