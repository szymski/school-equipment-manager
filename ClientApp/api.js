import axios from 'axios';
import { isString } from 'util';

export const api = {
    useDevVersion: true,
    loggedIn: false,
    authToken: null,
    loading: false,
    username: "SuperUser",
    teacherId: null,
    messageCount: 0,
    teachers: null,
    
    currentError: null,

    async updateUserInfo() {
        var data = (await axios.get("/api/General/GetUserInfo")).data;
        this.loggedIn = data.loggedIn;
        this.username = data.username;
        this.teacherId = data.teacherId;
        this.messageCount = data.messageCount;
    },
  
    async login(username, password) {
        var data = (await axios.post("/api/General/Login", {
            username: username,
            password: password,
        })).data;

        this.authToken = data.auth_token;
        this.loggedIn = true;

        localStorage.authToken = this.authToken;

        return data;
    },

    async logout() {
        localStorage.removeItem("authToken");
        this.loggedIn = false;
        this.authToken = null;
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

    async updateTeacher(id, name, surname, barcode, enableAccount, username, email) {
        return (await axios.post("/api/Teachers/Update/" + id, {
            id: id,
            name: name,
            surname: surname,
            barcode: barcode,
            enableAccount: enableAccount,
            username: username,
            email: email,
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
        if(isString(response))
            return response;

        var result = "";

        for(var key in response) {
            result += response[key][0] + "<br>";
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
};