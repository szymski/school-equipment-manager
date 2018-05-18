<template>
    <div v-if="api.currentError" class="ui negative message">
        <i class="close icon" @click="clearError"></i>
        <div class="header">{{ api.currentError.title }}</div>
        <p v-html="api.currentError.message"></p>
    </div>
</template>

<script>
export default {
    data() {
        return {
        }
    },

    methods: {
        clearError() {
            this.api.clearError();
        }
    },

    async created() {
        this.api.clearError();
    }
}
</script>

<style>
</style>
