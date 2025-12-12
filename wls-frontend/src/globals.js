export const globals = {
  apiUrl: process.env.VUE_APP_API_URL,
  defaultDate: new Date(Date.now()).toLocaleDateString("de-DE", {
    dateStyle: "medium",
  }),

  getLineColor(line) {
    const colors = {
      Tram: "red",
      Bus: "blue",
      Night: "purple",
      Misc: "grey",
    };
    if (line.type === "Metro") {
      if (line.id.includes("U1")) {
        return "#e40615";
      }
      if (line.id.includes("U2")) {
        return "#a962a4";
      }
      if (line.id.includes("U3")) {
        return "#ef7d00";
      }
      if (line.id.includes("U4")) {
        return "#00963d";
      }
      if (line.id.includes("U5")) {
        return "#9a377a";
      }
      if (line.id.includes("U6")) {
        return "#9d692d";
      }
    }
    return colors[line.type];
  },

  async fetch(url, method, body) {
    const options = {
      method: method,
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json;charset=UTF-8",
      },
    };
    if (body) {
      options.body = body ? JSON.stringify(body) : "{}";
    }

    const fullUrl = url.startsWith("/") ? this.apiUrl + url : url;

    try {
      const response = await fetch(fullUrl, options);
      if (!response.ok) return null;
      return response.json();
    } catch (err) {
      throw err instanceof TypeError ? "Keine Verbindung zur API" : err;
    }
  },

  ORDER_OPTIONS: [
    {
      label: "Startzeit - neueste zuerst",
      order_id: "StartedAtDesc",
    },
    {
      label: "Startzeit - älteste zuerst",
      order_id: "StartedAtAsc",
    },
    {
      label: "Endzeit - neueste zuerst",
      order_id: "EndedAtDesc",
    },
    {
      label: "Endzeit - älteste zuerst",
      order_id: "EndedAtAsc",
    },
  ],

  TYPE_OPTIONS: [
    {
      label: "Verspätungen",
      value: "Delay",
    },
    {
      label: "Verkehrsunfälle",
      value: "Accident",
    },
    {
      label: "Rettungseinsätze",
      value: "AmbulanceOperation",
    },
    {
      label: "Feuerwehreinsätze",
      value: "FireDepartmentOperation",
    },
    {
      label: "Polizeieinsätze",
      value: "PoliceOperation",
    },
    {
      label: "Falschparker",
      value: "ParkingOffender",
    },
    {
      label: "Schadhafte Fahrzeuge",
      value: "DefectiveVehicle",
    },
    {
      label: "Fahrleitungsschäden",
      value: "CatenaryDamage",
    },
    {
      label: "Gleisschäden",
      value: "TrackDamage",
    },
    {
      label: "Signalstörungen",
      value: "SignalDamage",
    },
    {
      label: "Weichenstörungen",
      value: "SwitchDamage",
    },
    {
      label: "Bauarbeiten",
      value: "ConstructionWork",
    },
    {
      label: "Demonstrationen",
      value: "Demonstration",
    },
    {
      label: "Veranstaltungen",
      value: "Event",
    },
    {
      label: "Witterungsbedingt",
      value: "Weather",
    },
    {
      label: "Sonstiges",
      value: "Misc",
    },
  ],

  LINE_TYPES: [
    {
      type_id: "Metro",
      title: "U-Bahn",
    },
    {
      type_id: "Tram",
      title: "Straßenbahn",
    },
    {
      type_id: "Bus",
      title: "Bus",
    },
    {
      type_id: "Night",
      title: "Nightline",
    },
    {
      type_id: "Misc",
      title: "Veraltet / sonstige",
    },
  ],

  VALUE_AXIS_OPTIONS: [
    {
      label: "Anzahl der Störungen",
      value: "Count",
    },
    {
      label: "Dauer der Störungen (Stunden)",
      value: "Duration",
    },
  ]
};
