<template>
  <div>

  </div>
</template>

<script>
export default {
  name: '',
  props: {

  },
  data() {
    return {
      rules: {
        // string: [
        // { required: true, message: '必填', trigger: 'blur' }
        // ]
      },
      keyValues: {

        genders: [
          {
            key: 'key',
            value: 'value'
          }
        ]
      }
    }
  },
  computed: {
    test() {
      return ''
    }
  },
  watch: {
    // test1: function(newv, oldv) {
    // }
  },
  created() {
    Promise.all([
      // this.$api.patientQueryList({ pageSize: 1000000, pageIndex: 1 })
    ]).then(res => {
      // this.patients = res[0].data
    })
  },
  methods: {

  }

}
</script>

<style>

</style>
