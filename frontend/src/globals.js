export const globals = {
  apiUrl: 'http://127.0.0.1:5000',
  async fetch (url, method, body) {
    const options = {
      method: method,
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json;charset=UTF-8'
      }
    }
    if (body) {
      options.body = body ? JSON.stringify(body) : '{}'
    }
    url = url.startsWith('/') ? this.apiUrl + url : url
    try {
      return (await fetch(url, options)).json()
    } catch (err) {
      throw err instanceof TypeError ? 'Keine Verbindung zur API' : err
    }
  }
}
