<template>
  <div>
    <el-row>
      <el-col :span="18">
        <snail-search-page-table
          search-api="userQueryPage"
          :search-fields="searchFields"
          :fields="fields"
          :search-rules="rules"
        >
        </snail-search-page-table>
      </el-col>
      <el-col :span="6">
        <snail-page-table
          ref="table"
          v-loading="loading"
          :rows="tableDatas"
          :fields="roleFields"
          :pagination="pagination"
          :multi-select="multiSelect"
        ></snail-page-table>
      </el-col>
    </el-row>
  </div>
</template>

<script>
export default {
  data() {
    return {
      tableDatas: [],
      submitApi: '',
      formData: {},
      visible: false,
      pagination: { currentPage: 1, pageSize: 15, total: 0 },
      loading: false,
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
    roleFields() {
      return [
        {
          name: 'name',
          label: '角色'
        }
      ]
    },
    fields() {
      return [
        {
          name: 'name',
          label: '姓名'
        },
        {
          name: 'account',
          label: '账号'
        },
        {
          name: 'phone',
          label: '电话'

        },
        {
          name: 'email',
          label: '邮箱'
        },
        {
          name: 'gender',
          label: '性别',
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
