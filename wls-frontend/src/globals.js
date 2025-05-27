export const globals = {
  apiUrl: 'http://127.0.0.1:5000',
  defaultDate: new Date(Date.now()).toLocaleDateString('de-DE', { dateStyle: 'medium' }),

  getLineColor (line) {
    const colors = ['#1c60a7', 'red', 'grey', 'purple']
    if (line.type === 2) {
      if (line.id.includes('U1')) { return '#e40615' }
      if (line.id.includes('U2')) { return '#a962a4' }
      if (line.id.includes('U3')) { return '#ef7d00' }
      if (line.id.includes('U4')) { return '#00963d' }
      if (line.id.includes('U5')) { return '#9a377a' }
      if (line.id.includes('U6')) { return '#9d692d' }
    }
    return colors[line.type]
  },

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
