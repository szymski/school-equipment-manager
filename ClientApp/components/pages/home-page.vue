<template>
    <div>
        <h1>Strona główna</h1>
        <p>Witaj na stronie systemu ewidencji inwentarzu szkolnego.</p>

        <div v-if="info" class="ui stackable two column equal width grid">
            <div class="ui column">
                <div class="ui segment dashboard-entry">
                    <div class="dashboard-entry-wrapper">
                        <p class="number">{{ info.totalItems }}</p>
                        <p class="description">
                            <a @click="goToItems">Liczba przedmiotów</a>
                        </p>
                    </div>
                </div>
            </div>
            <div class="ui column">
                <div class="ui segment dashboard-entry">
                    <div class="dashboard-entry-wrapper">
                        <p class="number">{{ info.borrowedItems }}</p>
                        <p class="description">
                            <a @click="goToUnreturnedItems">Niezwrócone przedmioty</a>
                        </p>
                    </div>
                </div>
            </div>

            <div class="ui column">
                <div class="ui segment dashboard-entry">
                    <div class="dashboard-entry-wrapper">
                        <p class="number">{{ info.borrowedTodayCount }}</p>
                        <p class="description">Liczba pobrań dzisiaj</p>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import router from "../../router.js";

export default {
    data() {
        return {
            info: null,
        }
    },

    methods: {
        goToItems() {
            router.push("/items");
        },

        goToUnreturnedItems() {
            router.push({
                name: "items",
                params: {
                    filterState: "borrowed"
                }
            });
        }
    },

    async created() {
        this.api.loading = true;

        this.info = await this.api.getDashboardInfo();

        this.api.loading = false;
    }
}
</script>

<style>
    .dashboard-entry-wrapper {
        text-align: center;
    }

    .dashboard-entry-wrapper .number {
        margin-bottom: 0;
        font-size: 80px;
    }

    .dashboard-entry-wrapper .description {
        font-size: 24px;
        margin-bottom: 0.5em;
        color: #222222;
    }

    .dashboard-entry-wrapper .description a {
        color: #222222;
    }

    .dashboard-entry-wrapper .description a:hover {
        color: #666666;
    }
</style>
