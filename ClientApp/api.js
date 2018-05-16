import axios from 'axios';

export const api = {
    username: "SuperUser",
    teachers: null,

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
    }
};