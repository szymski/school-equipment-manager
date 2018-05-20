<template>
    <div>
        <h1>Strona główna</h1>
        <p>Witaj na stronie systemu ewidencji inwentarzu szkolnego.</p>

        <div class="ui stackable equal width grid">
            <div class="ui column">
                <div class="ui segment">
                    <div class="dashboard-entry-wrapper">
                        <p class="number">17</p>
                        <p class="description">Liczba przedmiotów</p>
                    </div>
                </div>
            </div>
            <div class="ui column">
                <div class="ui segment">
                    <div class="dashboard-entry-wrapper">
                        <p class="number">5</p>
                        <p class="description">Niezwrócone przedmioty</p>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
export default {
    data() {
        return {
            
        }
    },
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
    }
</style>
