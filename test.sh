#!/bin/sh

curl -v  "http://localhost:4747/dir/Redirect.html" &>/dev/null & # 200 OK
curl -v  "http://localhost:4747/notfound.html" &>/dev/null & # 404 NotFound
curl -v  "http://localhost:4747/" &>/dev/null & # 200 OK
curl -v  "http://localhost:4747/aboutus2.html" &>/dev/null & # 200 OK
curl -v  "http://localhost:4747/aboutus.html" &>/dev/null & # 301 Redirect

