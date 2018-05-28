<template>
    <div>
        <h1>Przeczytaj wiadomość <template v-if="title">- {{ title }}</template></h1>

        <error-display/>

        <template v-if="title">
            <h5 class="ui top attached header">
                {{ date }}
            </h5>
            <div class="ui attached segment">
                {{ body }}
            </div>
        </template>
    </div>
</template>

<script>
import router from "../router.js";

export default {
    data() {
        return {
            messageId: this.$route.params.id,

            title: null,
            body: null,
            date: null,
        }
    },

    methods: {
        goToViewMessage(messageId) {
            router.push("/view-message/" + messageId);
        }
    },

    async created() {
        this.api.loading = true;

        try {
            var data = await this.api.getMessage(this.messageId);
            this.title = data.title;
            this.body = data.body;
            this.date = data.date;
        }
        catch(e) {
            this.api.displayError("Wystąpił błąd", this.api.parseError(e.response.data));
        }

        this.api.loading = false;
    }
}
</script>

<style>
    .unread-message {
        font-weight: bold;
    }
</style>
