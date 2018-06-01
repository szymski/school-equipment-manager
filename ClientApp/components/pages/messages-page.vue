<template>
    <div>
        <h1>Wiadomości</h1>

        <error-display/>

        <table class="ui celled table">
            <thead>
                <tr>
                    <th class="collapsing">lp.</th>
                    <th style="">Tytuł</th>
                    <th class="collapsing">Data</th>                    
                    <!-- <th class="collapsing"></th> -->
                </tr>
            </thead>
            <tbody>
                <tr v-for="(item, index) in messages" v-bind:key="index">
                    <td>{{ index + 1 }}</td>
                    <td>
                        <a :class="{ 'unread-message': !item.read }" @click="goToViewMessage(item.id)">{{ item.title }}</a>
                    </td>
                    <td class="single line">{{ item.date }}</td>
                    <!-- <td>
                        <button class="ui fluid small red button" @click="removeLocation(item.id, $event)">Usuń</button>
                    </td> -->
                </tr>
            </tbody>
        </table>
    </div>
</template>

<script>
import router from "../../router.js";

export default {
    data() {
        return {
            messages: [ ]
        }
    },

    methods: {
        goToViewMessage(messageId) {
            router.push("/view-message/" + messageId);
        }
    },

    async created() {
        this.api.loading = true;
        this.messages = await this.api.getMessages();
        this.api.loading = false;

        this.api.updateUserInfo();
    }
}
</script>

<style>
    .unread-message {
        font-weight: bold;
    }
</style>
