const https = require('https');
const fs = require('fs');
const path = require('path');

// Carrega as credenciais do servidor e da CA
const options = {
  key: fs.readFileSync(path.join(__dirname, 'cert/server.key')),
  cert: fs.readFileSync(path.join(__dirname, 'cert/server.crt')),
  ca: fs.readFileSync(path.join(__dirname, 'cert/ca.crt')),
  requestCert: true, // Solicita certificado do cliente
  rejectUnauthorized: true // Só aceita clientes autorizados pela CA
};

const server = https.createServer(options, (req, res) => {
  // Verifica se o cliente apresentou um certificado válido
  if (!req.client.authorized) {
    res.writeHead(401);
    return res.end('Certificado de cliente inválido ou ausente.\n');
  }

  res.writeHead(200);
  res.end('Olá! Você está autenticado com certificado de cliente.\n');
});

server.listen(8443, () => {
  console.log('Servidor HTTPS rodando na porta 8443');
});