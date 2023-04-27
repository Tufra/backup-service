from http.server import HTTPServer, BaseHTTPRequestHandler

PORT = 8000
HOST = '0.0.0.0'

class SimpleHTTPRequestHandler(BaseHTTPRequestHandler):

    def do_GET(self):
        print('received get')
        self.send_response(200)
        self.end_headers()
        with open('./share/index.html', 'rb') as f:
            content = f.read()
            if len(content) != 0:
                self.wfile.write(content)
            else:
                self.wfile.write(b'nothing found')
            


httpd = HTTPServer((HOST, PORT), SimpleHTTPRequestHandler)
print(f'listening at {HOST}:{PORT}')
httpd.serve_forever()