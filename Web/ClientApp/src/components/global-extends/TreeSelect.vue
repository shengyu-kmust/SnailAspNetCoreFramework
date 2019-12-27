<template>
    <el-select :value="valueTitle" :clearable="clearable" @clear="clearHandle" :placeholder="$t('common.select.a')">
        <el-option :value="valueTitle" :label="valueTitle">
            <el-tree  id="tree-option"
                      ref="selectTree"
                      :accordion="accordion"
                      :data="options"
                      :props="props"
                      :node-key="props.value"
                      @node-click="handleNodeClick">
            </el-tree>
        </el-option>
    </el-select>
</template>
<script>
    export default {
        name: 'TreeSelect',
        props: {
            /* 配置项 */
            props: {
                type: Object,
                default: () => {
                    return {
                        value: 'id',
                        label: 'name',
                        children: 'children'
                    };
                }
            },
            /* 选项列表数据(树形结构的对象数组) */
            options: {
                type: Array,
                default: () => { return []; }
            },
            /* 初始值 */
            value: {
                type: Number,
                default: () => { return null; }
            },
            /* 可清空选项 */
            clearable: {
                type: Boolean,
                default: () => { return true; }
            },
            /* 自动收起 */
            accordion: {
                type: Boolean,
                default: () => { return true; }
            },
        },
        data() {
            return {
                valueId: this.value,
                valueTitle: '',
            };
        },
        mounted() {
            this.initHandle();
        },
        methods: {
            // 初始化值
            initHandle() {
                if (this.valueId) {
                    this.valueTitle = this.$refs.selectTree.getNode(this.valueId).data[this.props.label]; // 初始化显示
                    this.$refs.selectTree.setCurrentKey(this.valueId); // 设置默认选中
                }
                this.$nextTick(() => {
                    const scrollWrap = document.querySelectorAll('.el-scrollbar .el-select-dropdown__wrap')[0];
                    const scrollBar = document.querySelectorAll('.el-scrollbar .el-scrollbar__bar');
                    scrollWrap.style.cssText = 'margin: 0px; max-height: none; overflow: hidden;';
                    scrollBar.forEach(e => {e.style.width = 0;});
                });
            },
            // 切换选项
            handleNodeClick(node) {
                this.valueTitle = node[this.props.label];
                this.valueId = node[this.props.value];
                this.$emit('getValue', this.valueId);
            },
            // 清除选中
            clearHandle() {
                this.valueTitle = '';
                this.valueId = null;
                this.clearSelected();
                this.$emit('getValue', null);
            },
            /* 清空选中样式 */
            clearSelected() {
                const allNode = document.querySelectorAll('#tree-option .el-tree-node');
                allNode.forEach((element) => element.classList.remove('is-current'));
            },
            getSelectData() {
                if (this.valueId == null) {
                    return null;
                }
                return {value: this.valueId, label: this.valueTitle};
            }
        },
        watch: {
            value() {
                this.valueId = this.value;
                this.initHandle();
            }
        },
    };
</script>

<style scoped>
    .el-scrollbar .el-scrollbar__view .el-select-dropdown__item{
        height: auto;
        max-height: 274px;
        padding: 0;
        overflow: hidden;
        overflow-y: auto;
    }
    .el-select-dropdown__item.selected{
        font-weight: normal;
    }
    ul li >>>.el-tree .el-tree-node__content{
        height:auto;
        padding: 0 20px;
    }
    .el-tree-node__label{
        font-weight: normal;
    }
    .el-tree >>>.is-current .el-tree-node__label{
        color: #409EFF;
        font-weight: 700;
    }
    .el-tree >>>.is-current .el-tree-node__children .el-tree-node__label{
        color:#606266;
        font-weight: normal;
    }
</style>

