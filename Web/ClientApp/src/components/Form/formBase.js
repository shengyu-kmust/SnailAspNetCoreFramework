/**
 * 表单通用的方法，所有用到form表单的地方都可以引用此公共js，进行混入，并重写相应的方法已实现自定义功能
 *
 */
export const FormBaseMixin = {
  props: {
    initFormData: {
      type: Object,
      default: () => ({})
    },
    fields: {
      type: Array,
      default: () => ([])
    },
    rules: {
      type: Object,
      default: () => ({})
    }
  },
  data() {
    // 读取通用配置
    const config = this.$config

    return {
      ComponentSize: config.componentSize(),
      validateFormName: 'form', // 可以在外部进行覆盖
      formData: {}
    }
  },
  created() {
    if (this.initFormData) {
      this.formData = this.$_.cloneDeep(this.initFormData)
    } else {
      this.formData = {}
    }
  },
  methods: {
    resetSearch() {
      this.$refs[this.validateFormName].resetFields()
    },
    validate(fn) {
      console.log('validate')
      this.$refs[this.validateFormName].validate(fn)
    }

  }
}

