import axios from 'axios';

export const api = {
    username: "SuperUser",

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

    async getTeacher(teacherId) {
        return (await axios.get("/api/Teachers/Get?id=" + teacherId)).data;
    },

};