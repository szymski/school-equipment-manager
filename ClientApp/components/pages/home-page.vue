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
                <div class="ui two column grid">
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
                                <p class="number">{{ info.borrowedItemsSelf }}</p>
                                <p class="description">
                                    <a @click="goToUnreturnedItemsSelf">Niezwrócone przez ciebie</a>
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="ui column">
                <div class="ui segment dashboard-entry">
                    <div class="dashboard-entry-wrapper">
                        <canvas id="typesChart" width="400" height="200"></canvas>
                        <p class="description" style="margin-top:1em;">
                            Liczba przedmiotów danego typu
                        </p>
                    </div>
                </div>
            </div>

            <div class="ui column">
                <div class="ui segment dashboard-entry">
                    <div class="dashboard-entry-wrapper">
                        <canvas id="borrowsChart" width="400" height="200"></canvas>
                        <p class="description" style="margin-top:1em;">
                            Liczba pobrań w danym dniu
                        </p>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import router from "../../router.js";
import { setTimeout } from 'timers';

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
        },

        goToUnreturnedItemsSelf() {
            router.push({
                name: "items",
                params: {
                    filterState: "borrowed-self"
                }
            });
        },

        prepareTypesChart() {
            setTimeout(() => {
                var ctx = document.getElementById("typesChart");

                var labels = [ ];
                var data = [ ];
                var colors = [ ];
                this.info.typesData.forEach(el => {
                    labels.push(el.templateName);
                    data.push(el.itemCount);
                    colors.push(this.api.generateColor("template" + el.template));
                });

                var chart = new Chart(ctx, {
                    type: 'doughnut',
                    data: {
                        labels: labels,
                        datasets: [{
                            data: data,
                            backgroundColor: colors,
                        }]
                    },
                    options: {
                        
                    }
                });
            }, 500);
        },

        prepareBorrowsChart() {
            setTimeout(() => {
                var ctx = document.getElementById("borrowsChart");

                var labels = [ ];
                var data = [ ];
                var colors = [ ];
                this.info.borrowsData.forEach(el => {
                    labels.push(el.date);
                    data.push(el.borrowCount);
                });

                var chart = new Chart(ctx, {
                    type: 'line',
                    data: {
                        labels: labels,
                        datasets: [{
                            label: "Pobrania",
                            backgroundColor: "transparent",
                            borderColor: "#34b2d1",
                            data: data,
                        }]
                    },
                    options: {
                        
                    }
                });
            }, 500);
        }
    },

    mounted() {
        
    },

    async created() {
        this.api.loading = true;

        this.info = await this.api.getDashboardInfo();
        console.log(this.info);

        this.prepareTypesChart();
        this.prepareBorrowsChart();

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
