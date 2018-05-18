import axios from 'axios';
import { isString } from 'util';

export const api = {
    loggedIn: false,
    username: "SuperUser",
    teachers: null,
    
    currentError: null,

    async updateItemIdentifier(itemId, identifier) {
        console.log(itemId);
        console.log(identifier);
        await axios.post("/api/Items/UpdateShortId", { id: itemId, identifier: identifier });
    },

    async getItem(itemId) {
        return (await axios.get("/api/Items/Get?id=" + itemId)).data;
    },

    async getLocations() {
        return (await axios.get("/api/Locations")).data;
    },

    async fetchTeachers() {
        if(this.teachers)
            return;

        console.log("Fetching teachers");
        var temp = (await axios.get("/api/Teachers")).data;
        this.teachers = { };
        temp.forEach(el => {
            this.teachers[el.id] = el;
        });
        console.log(this.teachers);
    },

    async getTeacher(teacherId) {
        return (await axios.get("/api/Teachers/Get?id=" + teacherId)).data;
    },

    async addTeacher(name, surname, barcode) {
        return await axios.post("/api/Teachers/Add", { name: name, surname: surname, barcode: barcode });
    },

    async getEvents(itemId) {
        return (await axios.get("/api/Items/Events/" + itemId)).data;
    },

    /// Returns whether the item hasn't been returned yet.
    isItemBorrowed(eventList) {
        var borrowed = false; // Has the item been borrowe

        eventList.forEach(i => {
            if(i.type.toLowerCase() == "pobrano" || i.type.toLowerCase() == "borrowed")
                borrowed = true;
            if(i.type.toLowerCase() == "zwrócono" || i.type.toLowerCase() == "returned")
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
    }
};