
<template>
  <div style="display:flex;flex:1">
    <snail-simple-crud
      search-api="demoQueryPage"
      add-api="demoSave"
      edit-api="demoSave"
      remove-api="demoRemove"
      :search-fields="searchFields"
      :fields="fields"
      :search-rules="rules"
      :form-rules="rules"
      :form-fields="fields"
    >
    </snail-simple-crud>
  </div>
</template>

<script>
export default {
  data() {
    return {
      keyValues: {
      },
      rules: {
        //name: [
        //{ required: true, message: '必填项', trigger: 'blur' }
        //]
      }
    }
  },
  computed: {
    fields() {
      return [
        {
          name: 'name',
          label: '姓名',
          type: 'string',
          span: 12
        },
        {
          name: 'gender',
          label: '性别',
          type: 'int',
          span: 12
        },
        {
          name: 'age',
          label: '年龄',
          type: 'int',
          span: 12
        },
        {
          name: 'birthday',
          label: '出生日期',
          type: 'datetime',
          span: 12
        },
        {
          name: 'enabled',
          label: '是否有效',
          type: 'string',
          span: 12
        },
      ]
    },
    searchFields() {
      return [
        {
          name: 'keyWord',
          label: '关键字',
          type: 'string'
        }
      ]
    }
  },
  created() {
  },
  methods: {
  }
}
</script>
