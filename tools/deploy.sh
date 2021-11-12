#!/bin/bash

set -e

basedir=$(cd $(dirname $0) ; pwd)/..

cd $basedir/src

rm -rf $basedir/dist
mkdir $basedir/dist

cp $basedir/src/CloudFormation.yaml $basedir/dist

dotnet publish -c release -o $basedir/dist

aws cloudformation package \
  --template-file $basedir/dist/CloudFormation.yaml \
  --output-template-file $basedir/dist/deploy.yaml \
  --s3-bucket pauloprsdesouza-dev \
  --s3-prefix cloud-formation/lab-chat

aws cloudformation deploy \
  --stack-name lab-chat \
  --template-file $basedir/dist/deploy.yaml \
  --parameter-overrides Environment=Development \
  --capabilities CAPABILITY_IAM \
