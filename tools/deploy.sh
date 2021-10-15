#!/bin/bash

set -e

aws cloudformation package \
  --template-file ./src/CloudFormation.yaml \
  --output-template-file ./dist/deploy.yaml \
  --s3-bucket pauloprsdesouza-dev \
  --s3-prefix cloud-formation/lab.chat

aws cloudformation deploy \
  --stack-name lab.chat \
  --template-file ./dist/deploy.yaml \
  --parameter-overrides Environment=Development \
