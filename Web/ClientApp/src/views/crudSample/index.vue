<template>
  <div>
    <snail-simple-crud
      search-api="getList"
      add-api="add"
      edit-api="edit"
      remove-api="remove"
      :search-fields="searchFields"
      :fields="fields"
      :search-rules="rules"
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
        yesnos: []
      },
      rules: {
        string: [
          { required: true, message: '请输入活动名称', trigger: 'blur' }
        ]
      }
    }
  },
  computed: {
    fields() {
      return [
        {
          name: 'string',
          label: '字符串',
          type: 'string',
          span: 12
        },
        {
          name: 'int',
          label: '数字',
          type: 'int',
          span: 12
        },
        {
          name: 'datetime',
          label: '时间',
          type: 'datetime',
          span: 12,
          formatter: this.timeFormatter

        },
        {
          name: 'select',
          label: '单选',
          type: 'select',
          span: 12,
          formatter: this.selectFormatter,
          keyValues: this.keyValues.yesnos
        },
        {
          name: 'multiSelect',
          label: '多选',
          type: 'multiSelect',
          span: 12,
          keyValues: this.keyValues.yesnos,
          formatter: this.selectFormatter
        }
      ]
    },
    searchFields() {
      return [
        {
          name: 'string',
          label: '字符串',
          type: 'string'
        },
        {
          name: 'int',
          label: '数字',
          type: 'int'
        },
        {
          name: 'datetime',
          label: '时间',
          type: 'datetime'
        },
        {
          name: 'select',
          label: '单选',
          type: 'select',
          keyValues: this.keyValues.yesnos
        },
        {
          name: 'multiSelect',
          label: '多选',
          type: 'multiSelect',
          keyValues: this.keyValues.yesnos
        }
      ]
    }
  },
  created() {
    this.keyValues.yesnos = [{
      key: 'yes',
      value: '是'
    },
    {
      key: 'no',
      value: '否'
    }]
  },
  methods: {
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
