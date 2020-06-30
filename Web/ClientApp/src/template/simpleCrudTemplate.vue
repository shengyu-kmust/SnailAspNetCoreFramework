<template>
  <div>
    <snail-simple-crud
      search-api=""
      add-api=""
      edit-api=""
      remove-api=""
      :search-fields="searchFields"
      :form-fields="fields"
      :fields="fields"
      :search-rules="rules"
      :form-rules="rules"
    >

    </snail-simple-crud>
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
        // xx:[
        //   {
        //     key:'',
        //     value:''
        //   }
        // ],
        // xx:this.$config.xxx
      }
    }
  },
  computed: {
    fields() {
      return [
        // {
        //   name: 'xx',
        //   label: '姓名',
        //   type: 'string',
        //   span: 8,
        //   keyValues:xx
        // }
      ]
    },
    searchFields() {
      return [
        // {
        //   name: 'string',
        //   label: '字符串',
        //   type: 'string'
        // }
      ]
    }
  },
  created() {
  },
  methods: {

  }

}
</script>

<style>

</style>
