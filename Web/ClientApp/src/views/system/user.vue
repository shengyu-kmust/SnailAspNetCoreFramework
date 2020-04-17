<template>
  <div>
    <snail-simple-crud
      search-api="userQueryPage"
      add-api="userSave"
      edit-api="userSave"
      remove-api="userRemove"
      :search-fields="searchFields"
      :fields="fields"
      :form-rules="rules"
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
          keyValues: this.$getKeyValue('EGender'),
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
      this.keyValues.genders = [
        {
          key: 'male',
          value: '男'
        }, {
          key: 'female',
          value: '女'
        }
      ]
    },
    selectFormatter(row, column, cellValue, index) {
      // return cellValue
      return this.$util.keyValueFormart(this.keyValues.yesnos, cellValue)
    },
    timeFormatter(row, column, cellValue, index) {
      return this.$dayjs(cellValue).format('YYYY-MM-DD')
    }
  }
}
</script>
