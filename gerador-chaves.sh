#!/bin/bash
set -e

# Gere a chave da CA em PKCS#8 PEM
openssl genpkey -algorithm RSA -out ca.key -pkeyopt rsa_keygen_bits:4096 -outform PEM

# Gere o certificado da CA (válido por 10 anos)
openssl req -x509 -new -nodes -key ca.key -sha256 -days 3650 -out ca.crt -subj "//CN=MyRootCA"

# Gere a chave do servidor em PKCS#8 PEM
openssl genpkey -algorithm RSA -out server.key -pkeyopt rsa_keygen_bits:4096 -outform PEM

# Gere o CSR do servidor
openssl req -new -key server.key -out server.csr -subj "//CN=localhost"

# Assine o certificado do servidor com a CA
openssl x509 -req -in server.csr -CA ca.crt -CAkey ca.key -CAcreateserial -out server.crt -days 3650 -sha256

# Gere a chave do cliente em PKCS#8 PEM
openssl genpkey -algorithm RSA -out client.key -pkeyopt rsa_keygen_bits:4096 -outform PEM

# Gere o CSR do cliente
openssl req -new -key client.key -out client.csr -subj "//CN=client"

# Assine o certificado do cliente com a CA
openssl x509 -req -in client.csr -CA ca.crt -CAkey ca.key -CAcreateserial -out client.crt -days 3650 -sha256

echo "Todos os certificados e chaves foram gerados com sucesso!"
echo ""
echo "Distribuição sugerida:"
echo "Cliente recebe: client.key, client.crt, ca.crt"
echo "Servidor recebe: server.key, server.crt, ca.crt"