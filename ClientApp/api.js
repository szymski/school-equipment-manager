import axios from 'axios';
import { isString } from 'util';

export const api = {
    useDevVersion: true,
    loading: false,
    
    loggedIn: false,
    authToken: null,
    username: "SuperUser",
    role: "",
    teacherId: null,
    messageCount: 0,
    isAdmin: false,
    isMod: false,

    teachers: null,
    
    currentError: null,

    async updateUserInfo() {
        var data = (await axios.get("/api/General/GetUserInfo")).data;
        this.loggedIn = data.loggedIn;
        this.username = data.username;
        this.teacherId = data.teacherId;
        this.messageCount = data.messageCount;
        this.role = data.role;

        this.isAdmin = this.role == "administrator";
        this.isMod = this.role == "administrator" || this.role == "moderator";
    },
  
    async login(username, password) {
        var data = (await axios.post("/api/General/Login", {
            username: username,
            password: password,
        })).data;

        this.authToken = data.auth_token;
        localStorage.authToken = this.authToken;

        await this.updateUserInfo();
        this.loggedIn = true;

        return data;
    },

    async logout() {
        this.loggedIn = false;
        this.authToken = null;
        localStorage.removeItem("authToken");
    },

    async getDashboardInfo() {
        return (await axios.get("/api/General/GeneralInfo")).data;
    },

    async updateItemIdentifier(itemId, identifier) {
        await axios.post("/api/Items/UpdateShortId", { id: itemId, identifier: identifier });
    },

    async updateItemNotes(itemId, notes) {
        await axios.post("/api/Items/UpdateNotes", { id: itemId, notes: notes });
    },

    async updateItemTemplate(itemId, templateId) {
        await axios.post("/api/Items/UpdateTemplate", { id: itemId, template: templateId });
    },

    async updateItemLocation(itemId, locationId) {
        await axios.post("/api/Items/UpdateLocation", { id: itemId, location: locationId });
    },

    async getItem(itemId) {
        return (await axios.get("/api/Items/Get?id=" + itemId)).data;
    },

    async getLocations() {
        return (await axios.get("/api/Locations")).data;
    },

    async getLocationsAsTable() {
        var temp = (await axios.get("/api/Locations")).data;

        var locations = { };

        temp.forEach(l => {
            locations[l.id] = l;
        });

        return locations;
    },

    async updateLocationName(locationId, newName) {
        await axios.post("/api/Locations/UpdateName", { id: locationId, name: newName });
    },

    async fetchTeachers() {
        if(this.teachers)
            return;

        var temp = (await axios.get("/api/Teachers")).data;
        this.teachers = { };
        temp.forEach(el => {
            this.teachers[el.id] = el;
        });
    },

    async getTeacher(teacherId) {
        return (await axios.get("/api/Teachers/Get?id=" + teacherId)).data;
    },

    async addTeacher(name, surname, barcode) {
        return (await axios.post("/api/Teachers/Add", { name: name, surname: surname, barcode: barcode })).data;
    },

    async updateTeacher(id, name, surname, barcode, enableAccount, username, email, role) {
        return (await axios.post("/api/Teachers/Update/" + id, {
            id: id,
            name: name,
            surname: surname,
            barcode: barcode,
            enableAccount: enableAccount,
            username: username,
            email: email,
            role: role,
        })).data;
    },

    async updateTeacherSelf(email, password) {
        return (await axios.post("/api/Teachers/UpdateSelf", {
            email: email,
            password: password,
        })).data;
    },

    async removeTeacher(id) {
        return await axios.post("/api/Teachers/Remove/" + id);
    },

    async resetPassword(teacherId) {
        return (await axios.post("/api/Teachers/ResetPassword/" + teacherId)).data;
    },

    async getEvents(itemId) {
        return (await axios.get("/api/Items/Events/" + itemId)).data;
    },

    /// Returns whether the item hasn't been returned yet.
    isItemBorrowed(eventList) {
        var borrowed = false; // Has the item been borrowe

        eventList.forEach(i => {
            if(i.type.toLowerCase() == "pobrano" || i.type.toLowerCase() == "borrow")
                borrowed = true;
            if(i.type.toLowerCase() == "zwrócono" || i.type.toLowerCase() == "return")
                borrowed = false;
        });

        return borrowed;
    },

    generateBarcodeForTeacher(firstName, lastName) {
        var barcode = "";

        barcode += firstName.substr(0, 3).toUpperCase();
        barcode += "-";
        barcode += lastName.substr(0, 3).toUpperCase();
        barcode += "-";
        barcode += Math.floor((Math.random() * 899999) + 100000);;

        var replaceSymbols = {
            "Ą": "A",
            "Ć": "C",
            "Ę": "E",
            "Ł": "L",
            "Ń": "N",
            "Ó": "O",
            "Ś": "S",
            "Ź": "Z",
            "Ż": "Z",
        };

        for(var key in replaceSymbols)
            barcode = barcode.replace(new RegExp(key, "g"), replaceSymbols[key]);

        return barcode;
    },

    /// Returns a formatted error message from the response
    parseError(response) {
        if(response.status == 403)
            return "Nie masz wystarczających uprawnień.";

        if(isString(response.data))
            return response.data;

        var result = "";

        for(var key in response.data) {
            result += response.data[key][0] + "<br>";
        }

        return result;
    },

    displayError(title, message) {
        this.currentError = {
            title: title,
            message: message,
        }
    },

    clearError() {
        this.currentError = null;
    },

    async getBarcodesForTeacher(teacherId) {
        return (await axios.get("/api/BarCode/GetBarcodesForTeacher/" + teacherId)).data;
    },

    async addItemEvent(itemId, teacherId, type) {
        return (await axios.post("/api/Items/AddEvent", {
           id: itemId,
           teacherId: teacherId,
           type: type,
        })).data;
    },

    async parseCode(code) {
        return (await axios.get("/api/BarCode/ParseCode?code=" + code)).data;
    },

    async getMessages() {
        return (await axios.get("/api/Messages")).data;
    },

    async getMessage(messageId) {
        return (await axios.get("/api/Messages/Get/" + messageId)).data;
    },

    async requestPasswordReset(email) {
        return (await axios.post("/api/General/RequestPasswordReset", {
            email: email,
        })).data;
    },

    async sendAlreadyBorrowedMessage(teacherId, borrowedTeacherId, itemId) {
        return (await axios.post("/api/Teachers/SendAlreadyBorrowedMessage", {
            teacherId: teacherId,
            borrowedTeacherId: borrowedTeacherId,
            itemId: itemId,
        })).data;
    },

    generateColor(text) {
        Math.seed = text.hashCode();
        var hue = Math.abs(Math.seededRandom(360, 0));

        var color = HSVtoRGB(hue, 0.8, 0.8);
        return rgbToHex(color.r, color.g, color.b);
    }
};

function HSVtoRGB(h, s, v) {
    var r, g, b, i, f, p, q, t;
    if (arguments.length === 1) {
        s = h.s, v = h.v, h = h.h;
    }
    i = Math.floor(h * 6);
    f = h * 6 - i;
    p = v * (1 - s);
    q = v * (1 - f * s);
    t = v * (1 - (1 - f) * s);
    switch (i % 6) {
        case 0: r = v, g = t, b = p; break;
        case 1: r = q, g = v, b = p; break;
        case 2: r = p, g = v, b = t; break;
        case 3: r = p, g = q, b = v; break;
        case 4: r = t, g = p, b = v; break;
        case 5: r = v, g = p, b = q; break;
    }
    return {
        r: Math.round(r * 255),
        g: Math.round(g * 255),
        b: Math.round(b * 255)
    };
}

function componentToHex(c) {
    var hex = c.toString(16);
    return hex.length == 1 ? "0" + hex : hex;
}

function rgbToHex(r, g, b) {
    return "#" + componentToHex(r) + componentToHex(g) + componentToHex(b);
}

String.prototype.hashCode = function() {
    var hash = 0, i, chr;
    if (this.length === 0) return hash;
    for (i = 0; i < this.length; i++) {
      chr   = this.charCodeAt(i);
      hash  = ((hash << 5) - hash) + chr;
      hash |= 0; // Convert to 32bit integer
    }
    return hash;
};

Math.seededRandom = function(max, min) {
    max = max || 1;
    min = min || 0;

    Math.seed = (Math.seed * 9301 + 49297) % 233280;
    var rnd = Math.seed / 233280.0;

    return min + rnd * (max - min);
}