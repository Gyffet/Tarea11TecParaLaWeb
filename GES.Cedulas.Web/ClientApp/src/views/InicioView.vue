<template>
    <div class="inicio">
        <br /><br /><br />
        <inicio></inicio>
    </div>
</template>

<script>
    // @ is an alias to /src
    import Utils from "@/services/utils";

    import inicio from "@/components/Inicio.vue";

    export default {
        name: "Inicio",
        components: {
            inicio
        },
        computed: {
            loggedIn() {
                return this.$store.state.auth.user == null ? false : true;
            },
            currentUser() {
                if (this.loggedIn)
                    return this.$store.state.auth.user;
                return false;
            },

            dgS() {
                return Utils.dgSigla(this.cveArea);
            }

        }
    };
</script>
