<template>
  <div>
    <el-button @click="getData">获取接口</el-button>
    <li v-for="(item,index) in data" :key="index">{{ item.name }}-{{ item.age }}</li>
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
      data: [],
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
  created() {
    Promise.all([
      // this.$api.patientQueryList({ pageSize: 1000000, pageIndex: 1 })
    ]).then(res => {
      // this.patients = res[0].data
    })
  },
  methods: {
    getData() {
      this.$api.getMockList().then(res => {
        this.data = res.data
      })
    }
  }

}
</script>

<style>

</style>
