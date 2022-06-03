<template>
    <div class="login">
        <login></login>
    </div>
</template>

<script>
    // @ is an alias to /src
    import login from "@/components/Login.vue";

    export default {
        name: "Login",
        components: {
            login
        }
    };
</script>
