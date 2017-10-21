# FileManageSystem
操作系统 - 文件管理系统模拟
## 一、项目概述
### 1.1需求分析
本项目旨在实现一个简单的文件管理系统。在内存中开辟一个空间作为文件存储器，模拟操作系统磁盘管理，在项目退出时，需要该文件系统的内容保存到磁盘上，以便下次可以将其回复到内存中来。一个简单的文件系统，包括目录和文件，每个目录和文件都有其共同的属性，因此首先需要实现目录结构的定义。此外，文件有其内容，文件内容存储使用链式存储方式。程序需要实现的功能为: 显示分级目录，以及显示某目录下的所以文件，以及基本的创建文件，文件夹，删除文件，文件夹，复制，粘贴，剪切文件，文件读写，本地存储恢复。

### 1.2系统功能
### 1.2.1基础功能
#### 1.2.1.1 格式化
	将磁盘格式化，目录，文件清空，恢复原始设置。
#### 1.2.1.2 创建、删除、显示目录
	在任意文件夹下可以创建目录，并更新UI，或者删除目录，删除目录会删除该目录下所有文件。目录以树形结构的方式来显示，可先点击树形目录切换到任意目录，显示该目录下所有文件及目录。
#### 1.2.1.3 读写、打开、关闭、删除文件
	对于文件，可双击或右键打开，或者选择其它方式打开，打开文件后显示文件内容，可对其进行编辑修改，保存，删除等操作。

#### 1.2.1.4 右键菜单显示
   选中文件目录，右键菜单显示可对文件进行基本操作，在空白处右键，则显示对当前文件夹所能进行的操作。
#### 1.2.1.5 查看文件、目录属性
	点击查看属性，或者右键点击文件或者目录，选择属性菜单，可显示选中文件或目录信息，若未选中文件，则显示当前目录信息。
#### 1.2.1.6 文件、目录重命名
	可对文件，或者目录进行重命名，重命名后更新文件路径以及子目录文件路径。
#### 1.2.1.7 文件保存、恢复目录
	关闭程序后，对目录，文件内容进行保存，重新打开程序是读取文件，恢复目录结构并显示，以及恢复文件内容。
#### 1.2.1.8 显示当前磁盘空间使用情况
	在程序底部显示程序磁盘空间使用情况。
### 1.2.2高级功能 
#### 1.2.2.1 复制、粘贴、剪切文件
	可对文件进行复制，复制过程中先缓存一个复制文件，在粘贴时更改复制文件的相关属性，文件内容重新开辟空间；剪切文件则删除原来文件。
#### 1.2.2.2 前进、后退，返回上层目录操作
	使用两个栈空间来记录前序操作文件夹，后续操作文件夹，可向前向后切换文件夹，也可返回上层目录。
#### 1.2.2.3 搜索目录操作
	实现搜索功能，现在当前目录级子目录下进行搜索，若没有结果，则在全文搜索文件，搜索结束显示搜索结果。
### 1.3开发环境
该系统使用C#  WPF进行开发,  VS 2015作为开发工具，运行在Window系统上。

## 二、关键技术
### 2.1 目录属性及结构设计
#### 2.1.1目录属性
如下图所示
  
#### 2.1.2 目录结构
	目录结构采用树状结构目录，任意高度，存在根目录，每个文件都有唯一路径名。
### 2.2 文件存储空间管理方式
	本系统采用链接分配方式，目录包括文件第一块指针，每一块512B，后四个字节记录下一块的指针，最后一块后四位记为-1，每次读到-1则标记文件读取结束。
### 2.3 空闲空间管理方式
	使用位向量管理空闲空间，0表示空闲，1表示已用。
