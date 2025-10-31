#!/bin/bash
set -e

# Caminhos dos arquivos de entrada
CERT="client.crt"
KEY="client.key"

# Arquivo combinado
COMBINED="client-combined.pem"
B64="client-combined.txt"

# Combina: primeiro o certificado, depois a chave
cat "$CERT" "$KEY" > "$COMBINED"

# Codifica em base64 e salva em dois arquivos
base64 "$COMBINED" > "$B64"

echo "Arquivo combinado criado: $COMBINED"
echo "Arquivo base64 criado: $B64"