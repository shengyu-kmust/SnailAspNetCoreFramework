<template>
  <div style="display:flex;flex:1">
    <snail-simple-crud
      search-api="userQueryPage"
      add-api="userSave"
      edit-api="userSave"
      remove-api="userRemove"
      :search-fields="searchFields"
      :fields="fields"
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
        genders: []
      },
      rules: {
        name: [
          { required: true, message: '必填项', trigger: 'blur' }
        ],
        account: [
          { required: true, message: '必填项', trigger: 'blur' }
        ]
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
          name: 'account',
          label: '账号',
          type: 'string',
          span: 12
        },
        {
          name: 'phone',
          label: '电话',
          type: 'string',
          span: 12

        },
        {
          name: 'email',
          label: '邮箱',
          type: 'string',
          span: 12
        },
        {
          name: 'gender',
          label: '性别',
          type: 'select',
          span: 12,
          keyValues: this.$config.genderKeyValue,
          formatter: this.selectFormatter
        }
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
    this.init()
  },
  methods: {
    init() {

    },
    selectFormatter(row, column, cellValue, index) {
      return this.$util.keyValueFormart(this.$config.genderKeyValue, cellValue)
    },
    timeFormatter(row, column, cellValue, index) {
      return this.$dayjs(cellValue).format('YYYY-MM-DD')
    }
  }
}
</script>
