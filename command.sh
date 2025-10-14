#!/bin/bash

# Defina a senha para os arquivos PFX
PFX_PASSWORD="SuaSenhaForteAqui"

# Gere a chave da CA
openssl genrsa -out ca.key 4096

# Gere o certificado da CA (válido por 10 anos)
openssl req -x509 -new -nodes -key ca.key -sha256 -days 3650 -out ca.crt -subj "//CN=MyRootCA"

# Gere a chave do servidor
openssl genrsa -out server.key 4096

# Gere o CSR do servidor
openssl req -new -key server.key -out server.csr -subj "//CN=localhost"

# Assine o certificado do servidor com a CA
openssl x509 -req -in server.csr -CA ca.crt -CAkey ca.key -CAcreateserial -out server.crt -days 3650 -sha256

# Gere o arquivo PFX do servidor
openssl pkcs12 -export -out server.pfx -inkey server.key -in server.crt -certfile ca.crt -password pass:$PFX_PASSWORD

# Gere a chave do cliente
openssl genrsa -out client.key 4096

# Gere o CSR do cliente
openssl req -new -key client.key -out client.csr -subj "//CN=client"

# Assine o certificado do cliente com a CA
openssl x509 -req -in client.csr -CA ca.crt -CAkey ca.key -CAcreateserial -out client.crt -days 3650 -sha256

# Gere o arquivo PFX do cliente
openssl pkcs12 -export -out client.pfx -inkey client.key -in client.crt -certfile ca.crt -password pass:$PFX_PASSWORD

echo "Todos os certificados e chaves foram gerados com sucesso!"