#!/bin/sh
TEST_FILE="test_urls.txt"

while read entry; do
	expected="$(echo $entry | cut -d ' ' -f 1)[$(echo $entry | cut -d ' ' -f 2)]"
	url="$(echo "$entry" | cut -d ' ' -f 3)"
	printf "Testing: %-50s Expected (%-20s Got [%s])\n" \
		$url $expected $(curl -o /dev/null -s -w "%{http_code}" $url) &
done < $TEST_FILE
