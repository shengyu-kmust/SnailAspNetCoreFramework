<template>
  <div>
    <snail-simple-crud
      search-api="roleQueryPage"
      add-api="roleSave"
      edit-api="roleSave"
      remove-api="roleRemove"
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
        genders: []
      },
      rules: {
        name: [
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
          label: '角色名',
          type: 'string',
          span: 12
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
      // return cellValue
      return this.$util.keyValueFormart(this.keyValues.yesnos, cellValue)
    },
    timeFormatter(row, column, cellValue, index) {
      return this.$dayjs(cellValue).format('YYYY-MM-DD')
    }
  }
}
</script>
