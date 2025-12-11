#!/bin/bash
TARGET="https://bp-calculator-qa-cbgkg0bfdrdbf4c8.norwayeast-01.azurewebsites.net"
echo "Running OWASP ZAP Baseline Scan against $TARGET"
mkdir -p zap-output
docker run --rm \
  -v $(pwd)/zap-output:/zap/wrk \
  ghcr.io/zaproxy/zaproxy:stable zap-baseline.py \
  -t $TARGET \
  -J zap-report.json \
  -w zap-warn.html \
  -r zap-report.html
echo "OWASP ZAP Scan Completed"